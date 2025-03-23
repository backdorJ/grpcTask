import { useState } from "react";
import {AuthServiceClient} from "../../generated/auth_grpc_web_pb";
import {RegisterRequest} from "../../generated/auth_pb";
import './RegisterPage.css';
import {useNavigate} from "react-router-dom";

export default function RegisterPage() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [error, setError] = useState("");
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");

        if (password !== confirmPassword) {
            setError("Пароли не совпадают");
            return;
        }

        try {
            const client = new AuthServiceClient(process.env.REACT_APP_API_URL, null, null);
            const request = new RegisterRequest();
            request.setEmail(email);
            request.setPassword(password);
            request.setConfirmpassword(confirmPassword);

            try {
                const response = await client.register(request, {}, (err, response) => {
                    if (err) {
                        alert("Error during registration:", err);
                    } else {
                        console.log("Registration successful:", response);
                        navigate("/login");
                    }
                });
            } catch (error) {
                console.error("Error during registration:", error);
            }
        } catch (error) {
            console.error('Error during registration:', error);
        }
    };

    return (
        <div className="container">
            <div className="form-container">
                <h2 className="title">Регистрация</h2>
                {error && <p className="error-message">{error}</p>}
                <form onSubmit={handleSubmit} className="form">
                    <div className="input-group">
                        <label>Email</label>
                        <input
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                        />
                    </div>
                    <div className="input-group">
                        <label>Пароль</label>
                        <input
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                        />
                    </div>
                    <div className="input-group">
                        <label>Подтвердите пароль</label>
                        <input
                            type="password"
                            value={confirmPassword}
                            onChange={(e) => setConfirmPassword(e.target.value)}
                            required
                        />
                    </div>
                    <button type="submit" className="submit-button">Зарегистрироваться</button>
                </form>
                <button onClick={() => navigate("/login")}>Login</button>
            </div>
        </div>
    );
}