import { jwtDecode } from "jwt-decode";
import axios from "axios";
import { API_BASE_URL, ENDPOINTS } from "../config";

class AuthService {
  async login(username, password) {
    try {
      console.log("Logging in with email:", username);
      const response = await axios.post(`${API_BASE_URL}${ENDPOINTS.LOGIN}`, { 
        username, 
        password 
      });
      
      if (response.data.token) {
        const userData = {
          token: response.data.token,
          user: response.data.user || response.data,
          role: response.data.role || response.data.user?.role
        };
        localStorage.setItem("user", JSON.stringify(userData));
        console.log("Login successful:", userData);
        return { success: true, data: userData };
      }
      
      return { success: false, message: "No token received" };
    } catch (error) {
      console.error("Login error:", error);
      const message = error.response?.data?.message || "Login failed";
      return { success: false, message };
    }
  }

  logout() {
    localStorage.removeItem("user");
  }

  getCurrentUser() {
    const stored = localStorage.getItem("user");
    if (!stored) return null;

    try {
      const userData = JSON.parse(stored);
      if (userData.token) {
        const decoded = jwtDecode(userData.token);
        return {
          ...decoded,
          name: decoded.name || decoded.sub || userData.user?.name,
          email: decoded.email || userData.user?.email,
          role: decoded.role || userData.role || userData.user?.role
        };
      }
      return userData.user || userData;
    } catch (err) {
      console.error("Error decoding token:", err);
      return null;
    }
  }

  isTokenExpired(token) {
    try {
      const decoded = jwtDecode(token);
      return decoded.exp * 1000 < Date.now();
    } catch {
      return true;
    }
  }

  getUserRole() {
    const user = this.getCurrentUser();
    return user?.role || null;
  }

  isLoggedIn() {
    const stored = localStorage.getItem("user");
    if (!stored) return false;

    try {
      const { token } = JSON.parse(stored);
      return token && !this.isTokenExpired(token);
    } catch {
      return false;
    }
  }

  getToken() {
    const stored = localStorage.getItem("user");
    if (!stored) return null;

    try {
      const { token } = JSON.parse(stored);
      return token;
    } catch {
      return null;
    }
  }
}

// ✅ Export a singleton instance
export default new AuthService();