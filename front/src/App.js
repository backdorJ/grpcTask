import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import { useSelector } from "react-redux";
import RegisterPage from "./pages/registerPage/RegisterPage";
import LoginPage from "./pages/loginPage/LoginPage";
import ChatPage from "./pages/chatPage/ChatPage";

function App() {
    const isAuthenticated = useSelector((state) => state.auth.isAuthenticated);
    console.log(isAuthenticated);

    return (
            <Routes>
                <Route path="/login" element={isAuthenticated ? <ChatPage /> : <LoginPage />} />
                <Route path="/register" element={isAuthenticated ? <ChatPage /> : <RegisterPage />} />
                <Route path="*" element={<Navigate to={isAuthenticated ? "/chat" : "/login"} replace />} />
                <Route path="/chat" element={isAuthenticated ? <ChatPage /> : <LoginPage />} />
            </Routes>
    );
}

export default App;
