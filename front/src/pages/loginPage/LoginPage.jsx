import { useState } from "react";
import "./LoginPage.css";
import {useNavigate} from "react-router-dom";
import {AuthServiceClient} from "../../generated/auth_grpc_web_pb";
import {LoginRequest} from "../../generated/auth_pb";
import {useDispatch} from "react-redux";
import {login} from "../../stores/slices/authSlice";

export default function LoginPage() {
    const dispatch = useDispatch();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const navigate = useNavigate();

    const handleSubmit = (e) => {
        e.preventDefault();
        setError("");

        const client = new AuthServiceClient(process.env.REACT_APP_API_URL, null, null);
        const request = new LoginRequest();
        request.setEmail(email);
        request.setPassword(password);

        client.login(request, {}, (err, response) => {
            if (err) {
                console.error(err);
            }
            else {
                console.log(response);
                localStorage.setItem("username", email);
                localStorage.setItem("token", response.u[0]);
                dispatch(login(response.u[0]))
            }
        })
    };

    return (
        <div className="container">
            <div className="form-container">
                <h2 className="title">Вход</h2>
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
                    <button type="submit" className="submit-button">Войти</button>
                    <button onClick={() => navigate('/register')}>Register</button>
                </form>
            </div>
        </div>
    );
}
