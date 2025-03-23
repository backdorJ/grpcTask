import {createSlice} from "@reduxjs/toolkit";

const initialState = {
    isAuthenticated: !!localStorage.getItem("token"), // Загружаем токен из localStorage
    token: localStorage.getItem("token") || "",
};

const authSlice = createSlice({
    name: "auth",
    initialState: initialState,
    reducers: {
        login: (state, action) => {
            state.isAuthenticated = !!localStorage.getItem("token");
            state.token = action.payload;
        },
        logout: (state) => {
            state.isAuthenticated = false;
            localStorage.removeItem("token");
        }
    }
});

export const {login, logout} = authSlice.actions;
export default authSlice.reducer;