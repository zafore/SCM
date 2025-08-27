import { Navigate } from "react-router-dom";
import AuthServices from "../services/AuthServices"

export default function PrivateRoute({ children }) {
  const user = AuthServices.getCurrentUser();

  if (!user || !user.token || AuthServices.isTokenExpired(user.token)) {
    AuthServices.logout();
    return <Navigate to="/login" replace />;
  }

  return children;
}