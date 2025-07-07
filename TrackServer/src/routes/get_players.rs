use std::ffi::CStr;
use rocket::get;
use rocket::http::Status;
use rocket::response::status;
use rocket::serde::json::Json;
use serde_json::Value;
use crate::GET_PLAYERS;

#[get("/get_players")]
pub fn call_get_players() -> Result<status::Custom<Json<Value>>, status::Custom<String>> {
    unsafe {
        if let Some(cb) = GET_PLAYERS {
            let ptr = cb();
            if !ptr.is_null() {
                let c_str = CStr::from_ptr(ptr);
                if let Ok(json) = c_str.to_str() {
                    println!("[Rust] JSON from C#: {}", json);
                    match serde_json::from_str::<serde_json::Value>(json) {
                        Ok(val) => Ok(status::Custom(Status::Ok, Json(val))),
                        Err(_) => {
                            println!("Failed to parse json");
                            Err(status::Custom(Status::InternalServerError, String::from("Invalid JSON")))
                        }
                    }
                }else{
                    Err(status::Custom(Status::InternalServerError, String::from("Couldn't receive c_str")))
                }
            }else{
                Err(status::Custom(Status::InternalServerError, String::from("Failed to load pointer")))
            }
        } else {
            println!("No callback fn");
            Err(status::Custom(Status::InternalServerError, String::from("No callback fn")))
        }
    }
}