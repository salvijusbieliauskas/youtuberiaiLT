import { useContext, useEffect, useRef, useState } from "react";
import {
  addCategoryToChannel,
  removeCategoryFromChannel,
  removeChannel,
} from "../../../api";
import Card from "../Card";
import Category from "../../Category/Category";
import { AppContext } from "../../../context/AppContext";
import { ModerationContext } from "../../../context/ModerationContext";

function ModerationCard({ channel }) {
  const { getChannelsData, categories } = useContext(AppContext);
  const [channelData, setChannelData] = useState(channel);
  const [channelCategories, setChannelCategories] = useState(
    channelData.categories
  );
  const filteredCategories = categories.filter(
    (category) =>
      !channelCategories.some((channelCat) => category.name === channelCat.name)
  );

  const { secretPhrase } = useContext(ModerationContext);

  async function handleAddCategoryToChannel(channelId, category) {
    const response = await addCategoryToChannel(
      channelId,
      category,
      secretPhrase
    );
    if (response !== null) {
      setChannelData(response);
    }
  }
  async function handleRemoveCategoryToChannel(channelId, category) {
    const response = await removeCategoryFromChannel(
      channelId,
      category,
      secretPhrase
    );
    if (response !== null) {
      setChannelData(response);
    }
  }

  async function handleRemoveChannel(channelId, channelUrl) {
    const confirm = window.confirm(`Ištrinti ${channelUrl} kanalą?`);
    if (confirm) {
      const removed = await removeChannel(channelId, secretPhrase);
      if (removed) {
        await getChannelsData();
      }
    }
  }

  useEffect(() => {
    setChannelCategories(channelData.categories);
  }, [channelData]);

  return (
    <div className="flex gap-2 justify-center">
      <div className="relative w-full">
        <button
          onClick={() => handleRemoveChannel(channel.id, channel.customUrl)}
          className="absolute rounded-t-full  px-3 border-t-2 border-x-2 border-red-500 top-[-26px] left-24  bg-rose-100 hover:border-orange-500 hover:text-white hover:bg-rose-700 z-10"
        >
          Panaikinti
        </button>
        <Card channel={channelData} />
      </div>
      <div className="pl-4 py-2 w-[70%] text-white flex flex-wrap h-full gap-2 rounded-2xl border-2 border-orange-500  ">
        {filteredCategories.map((category) => (
          <div
            key={category.name}
            className="rounded-full pl-2 border border-white bg-gray-200 text-black flex items-center"
          >
            <Category category={category} />
            <button
              key={category.name}
              onClick={() =>
                handleAddCategoryToChannel(channel.id, category.name)
              }
              className="bg-[#42a85a] hover:bg-[#55c56f] active:bg-[#67d881]  ml-1 px-2 rounded-r-full"
            >
              +
            </button>
          </div>
        ))}
        {channelCategories.map((category) => (
          <div
            key={category.name}
            className="rounded-full pl-2 border border-white bg-gray-200 text-black flex items-center"
          >
            <Category category={category} />
            <button
              key={category.name}
              onClick={() =>
                handleRemoveCategoryToChannel(channel.id, category.name)
              }
              className="bg-rose-600 hover:bg-red-500 active:bg-red-400 ml-1 px-2 rounded-r-full"
            >
              X
            </button>
          </div>
        ))}
      </div>
    </div>
  );
}
export default ModerationCard;
