import React, { useEffect, useState } from "react";
import { ChatServiceClient } from '../../generated/chat_grpc_web_pb';
import { ChatMessage, Empty, MessageRequest } from "../../generated/chat_pb";

const client = new ChatServiceClient(process.env.REACT_APP_API_URL, null, {});

const ChatPage = () => {
    const [messages, setMessages] = useState([]);
    const [message, setMessage] = useState("");

    useEffect(() => {
        let isMounted = true;
        let retryTimeout = null;

        // Функция для подключения к стриму
        const connectStream = () => {
            const stream = client.receiveMessages(new Empty(), {
                "Authorization": "Bearer " + localStorage.getItem("token"),
            });

            stream.on("data", (msg) => {

                if (msg.u[0] == "test server")
                {

                }
                else {
                    if (!isMounted) return;
                    setMessages((prev) => [...prev, { user: msg.getUser(), text: msg.getText() }]);
                }
            });

            stream.on("error", (err) => {
                console.error("Ошибка в потоке:", err);
                if (isMounted) {
                    console.log("Переподключение через 3 секунды...");
                    retryTimeout = setTimeout(connectStream, 3000);
                }
            });

            stream.on("end", () => {
                console.log("Поток закрыт, переподключение...");
                if (isMounted) {
                    retryTimeout = setTimeout(connectStream, 3000);
                }
            });

            return stream;
        };

        const stream = connectStream();

        // Очистка при размонтировании компонента
        return () => {
            isMounted = false;
            stream.cancel(); // Закрытие потока
            if (retryTimeout) clearTimeout(retryTimeout); // Очищаем таймер
        };
    }, []);

    const sendMessage = () => {
        if (!message.trim()) return;

        const msg = new MessageRequest();
        msg.setUser(localStorage.getItem("username"));
        msg.setText(message);

        client.sendMessage(msg, {
            "Authorization": "Bearer " + localStorage.getItem("token"),
        }, (err, _) => {
            if (err) {
                console.error("Ошибка при отправке:", err);
            } else {
                setMessage("");
            }
        });
    };

    return (
        <div>
            <div>
                {messages.map((msg, i) => (
                    <p key={i}><strong>{msg.user}:</strong> {msg.text}</p>
                ))}
            </div>
            <input value={message} onChange={(e) => setMessage(e.target.value)} />
            <button onClick={sendMessage}>Отправить</button>
        </div>
    );
};

export default ChatPage;
