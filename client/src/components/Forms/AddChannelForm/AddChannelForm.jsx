import { useContext, useState } from "react";
import { addChannel } from "../../../api";
import { AppContext } from "../../../context/AppContext";
import { ModerationContext } from "../../../context/ModerationContext";

function AddChannelForm() {
  const [channelId, setChannelId] = useState("");
  const { setChannels, channels } = useContext(AppContext);
  const { secretPhrase } = useContext(ModerationContext);
  async function handleAddChannel(e) {
    e.preventDefault();
    const response = await addChannel(channelId, secretPhrase);
    if (response !== null) {
      setChannelId("");
      setChannels([response, ...channels]);
    }
  }
  function handleChannelIDInputChange(e) {
    setChannelId(e.target.value);
  }
  return (
    <form
      onSubmit={handleAddChannel}
      className="rounded-full bg-slate-300 text-xl flex"
    >
      <input
        value={channelId}
        onChange={handleChannelIDInputChange}
        className="rounded-l-full py-1 bg-slate-100 px-3 w-full"
        type="text"
        placeholder="Kanalo ID..."
      />
      <button
        type="submit"
        className=" px-2 border-l border-black rounded-r-full  hover:bg-emerald-200 active:bg-emerald-400 "
      >
        Pridėti
      </button>
    </form>
  );
}
export default AddChannelForm;
