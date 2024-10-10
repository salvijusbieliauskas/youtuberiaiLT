import { useContext, useState } from "react";
import { authorize } from "../../../api";
import { ModerationContext } from "../../../context/ModerationContext";

function AuthorizationForm() {
  const [phrase, setPhrase] = useState("");
  const { setAuthorized, setSecretPhrase } = useContext(ModerationContext);
  async function handleSubmit(e) {
    e.preventDefault();
    const result = await authorize(phrase);
    setAuthorized(result);
    if (result === true) {
      setSecretPhrase(phrase);
    }
  }
  return (
    <form
      onSubmit={handleSubmit}
      className="rounded-full bg-slate-300 text-xl flex"
    >
      <input
        value={phrase}
        onChange={(e) => setPhrase(e.target.value)}
        className="rounded-l-full py-1 bg-slate-100 px-3 w-full"
        type="text"
        placeholder="Frazė..."
      />
      <button
        type="submit"
        className=" px-2 border-l border-black rounded-r-full  hover:bg-emerald-200 active:bg-emerald-400 "
      >
        Autentikuoti
      </button>
    </form>
  );
}
export default AuthorizationForm;
