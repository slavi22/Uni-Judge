import { RouterProvider } from "react-router";
import { router } from "@/app/router.tsx";
import { Toaster } from "@/components/ui/sonner.tsx";

// bulletproof-react folder structure => https://github.com/alan2207/bulletproof-react/blob/master/docs/project-structure.md

function App() {
  return (
    <>
      <RouterProvider router={router} />
      <Toaster richColors visibleToasts={1} closeButton />
    </>
  );
}

export default App;
