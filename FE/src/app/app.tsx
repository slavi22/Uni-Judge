import { RouterProvider } from "react-router";
import { router } from "@/app/router.tsx";

// bulletproof-react folder structure => https://github.com/alan2207/bulletproof-react/blob/master/docs/project-structure.md

function App() {
  return <RouterProvider router={router} />;
}

export default App;
