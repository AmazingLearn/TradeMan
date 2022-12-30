import React from "react";

export function getUserId(): string {
    return sessionStorage.getItem("userId");
}

export function clearUserId() {
    sessionStorage.removeItem("userId");
}