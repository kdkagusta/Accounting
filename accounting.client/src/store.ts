import { configureStore } from "@reduxjs/toolkit";
import dashboardReducer from "./feature/dashboard/dashboardSlice";

export const store = configureStore({
    reducer: {
        dashboard: dashboardReducer,
        darkMode: darkModeReducer,
    },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;