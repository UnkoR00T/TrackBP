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

// Keeps track of connected clients. Sometimes
pub static GLOBAL_CLIENTS: Lazy<ClientList> = Lazy::new(|| Arc::new(Mutex::new(Vec::new())));

#[get("/ws")]
// Client HTTP to WS upgrade function
pub fn ws_compose(ws: WebSocket) -> Stream!['static] {
    let clients = GLOBAL_CLIENTS.clone();
    ws.stream(move |mut rx_stream: SplitStream<_>| {
        let (tx, mut rx) = unbounded_channel::<String>();
        {
            // Adding client after connection
            let mut locked = clients.lock().unwrap();
            locked.push(tx);
        }
        stream! {
            // Main message loop. For later client to server communication.
            // Useless for now.
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
            // Removing client after disconnection.
            let mut locked = clients.lock().unwrap();
            locked.retain(|sender| !sender.is_closed());
        }
    })
}

/// Broadcast message from server to all connected clients.
/// message should be parsed json but any String is fine.
pub fn broadcast_message(message: String) {
    let clients = GLOBAL_CLIENTS.clone();
    let clients = clients.lock().unwrap();
    for tx in clients.iter() {
        let _ = tx.send(message.clone());
    }
}
