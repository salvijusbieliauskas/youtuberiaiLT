import { Route, Router, Switch } from "wouter";
import Home from "./pages/Home";
import Moderation from "./pages/Moderation";
import { useEffect } from "react";
import { getChannels } from "./api";
import { ModerationProvider } from "./context/ModerationContext";

// bg-[#0f0f0f]
function App() {
  return (
    <main className="min-h-full bg-[#1c2124] text-[#101010]">
      <Router base="/">
        <Switch>
          <Route path="/">
            <Home />
          </Route>
          <Route path="/electrical">
            <ModerationProvider>
              <Moderation />
            </ModerationProvider>
          </Route>
          <Route path="/*">
            <div className="w-full flex justify-center text-center text-4xl pt-24 text-white">
              Page Not Found
            </div>
          </Route>
        </Switch>
      </Router>
    </main>
  );
}

export default App;
