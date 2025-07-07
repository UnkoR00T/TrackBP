mod routes;
mod cors;

use std::ffi::CStr;
use std::os::raw::c_char;
use rocket::{get, routes, Build, Config, Response, Rocket};
use std::{fs, thread};
use std::time::Duration;
use rocket::http::Status;
use rocket::serde::json::Json;
use rocket::response::status;
use serde_json::{json, Value};
use crate::cors::{preflight_cors, CORS};
use crate::routes::get_players::call_get_players;
use crate::routes::ws::{broadcast_message, ws_compose};

fn build_rocket() -> Rocket<Build> {
    // Reading BP settings.json and parsing it to JSON
    let file = fs::read_to_string("settings.json").expect("File not found, cannot bind to port!");
    let json: Value = json5::from_str(&file).unwrap();
    let config = Config {
        address: "0.0.0.0".parse().unwrap(),
        port: json.get("port").unwrap().as_i64().unwrap() as u16,
        ..Config::default()
    };
    // Main Rocket router
    rocket::custom(config)
        .attach(CORS)
        .mount("/",
               routes![preflight_cors, call_get_players, ws_compose])
}

#[unsafe(no_mangle)]
/// Main lib function.
pub extern "C" fn init_server() {
    thread::spawn(|| {
        println!("Setting up rocket server...");
        let _ = rocket::async_main(async {
            build_rocket().launch().await.expect("Rocket went crazy :thumbs up:");
        });
    });
}

// Get Players data binding
type GetPlayersFn = extern "C" fn() -> *const c_char;
static mut GET_PLAYERS: Option<GetPlayersFn> = None;
#[unsafe(no_mangle)]
pub extern "C" fn register_get_players(cb: GetPlayersFn) {
    unsafe {
        println!("Registered getPlayersFunction");
        GET_PLAYERS = Some(cb);
    }
}

// Get RaceInfo data binding
type GetRaceInfoFn = extern "C" fn() -> *const c_char;
static mut GET_RACEINFO: Option<GetPlayersFn> = None;
#[unsafe(no_mangle)]
pub extern "C" fn register_get_raceinfo(cb: GetRaceInfoFn) {
    unsafe {
        println!("Registered getRaceInfo");
        GET_RACEINFO = Some(cb);
    }
}

#[unsafe(no_mangle)]
/// Extern function for c# to broadcast messages to clients look ws.rs.
pub extern "C" fn send_message(json_ptr: *const c_char) {
    if json_ptr.is_null() {
        // In case of empty message
        eprintln!("Empty json pointer.");
        return;
    }

    let c_str = unsafe { CStr::from_ptr(json_ptr) };
    let json_str = match c_str.to_str() {
        Ok(s) => s.to_owned(),
        Err(_) => {
            eprintln!("Parse error.");
            return;
        }
    };
    broadcast_message(json_str);
}

