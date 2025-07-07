use rocket::{get};
use rocket_ws::{WebSocket, Stream, Message};
use std::sync::{Arc, Mutex};
use futures::{SinkExt, StreamExt};
use futures_util::stream::SplitStream;
use once_cell::sync::Lazy;
use tokio::sync::mpsc::{UnboundedSender, unbounded_channel};
use async_stream::stream;
use rocket_ws::result::Error;

type ClientList = Arc<Mutex<Vec<UnboundedSender<String>>>>;
pub static GLOBAL_CLIENTS: Lazy<ClientList> = Lazy::new(|| Arc::new(Mutex::new(Vec::new())));

#[get("/ws")]
pub fn ws_compose(ws: WebSocket) -> Stream!['static] {
    let clients = GLOBAL_CLIENTS.clone();
    ws.stream(move |mut rx_stream: SplitStream<_>| {
        let (tx, mut rx) = unbounded_channel::<String>();
        {
            let mut locked = clients.lock().unwrap();
            locked.push(tx);
        }
        stream! {
            loop {
                tokio::select! {
                    Some(msg) = rx.recv() => {
                        yield Ok(Message::Text(msg));
                    }
                    Some(result) = rx_stream.next() => {
                        match result {
                            Ok(msg) => {
                            }
                            Err(_) => {
                                break;
                            }
                        }
                    }
                    else => {
                        break;
                    }
                }
            }
            let mut locked = clients.lock().unwrap();
            locked.retain(|sender| !sender.is_closed());
        }
    })
}
pub fn broadcast_message(message: String) {
    let clients = GLOBAL_CLIENTS.clone();
    let clients = clients.lock().unwrap();
    for tx in clients.iter() {
        let _ = tx.send(message.clone());
    }
}
