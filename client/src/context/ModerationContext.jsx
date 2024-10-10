import { createContext, useState } from "react";

export const ModerationContext = createContext(null);
export function ModerationProvider({ children }) {
  const [secretPhrase, setSecretPhrase] = useState("");
  const [authorized, setAuthorized] = useState(false);

  return (
    <ModerationContext.Provider
      value={{
        secretPhrase,
        authorized,
        setSecretPhrase,
        setAuthorized,
      }}
    >
      {children}
    </ModerationContext.Provider>
  );
}
