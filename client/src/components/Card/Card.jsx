import { useEffect, useState } from "react";
import linkSVG from "../../assets/icons/link.svg";
import refreshSVG from "../../assets/icons/refresh.svg";
import Category from "../Category/Category";
import { updateChannel } from "../../api";
import Loader from "../Loader/Loader";
import { formatNumber } from "../../helpers/NumberFormating";

function Card({ channel }) {
  const [channelData, setChannelData] = useState(channel);
  const [loading, setLoading] = useState(false);
  // let channelData = channel;
  async function handleUpdateClick(channelId) {
    setLoading(true);
    const response = await updateChannel(channelId);
    if (response !== null) {
      setChannelData(response);
    }
    setLoading(false);
  }
  useEffect(() => {
    setChannelData(channel);
  }, [channel]);
  return (
    <div className="bg-gray-dark rounded-b-3xl rounded-t-4xl w-full ">
      <div className="flex bg-gray-light text-black rounded-3xl pt-2 gap-4 w-full relative">
        <img
          className="rounded-3xl object-cover w-24 h-24 sm:w-28 sm:h-28 ml-2 mb-2"
          src={channelData.thumbnail}
          alt=""
          referrerPolicy="no-referrer"
        />
        <div className="mb-2 mr-2 ">
          <div className="flex gap-2 mb-1 ">
            <div className="sm:flex sm:gap-2">
              <a
                className="flex sm:block relative"
                href={channelData.customUrl}
                target="_blank"
              >
                <h2 className="mt-2 sm:mt-[2px] text-2xl font-bold inline">
                  {channelData.title}
                </h2>
                <img
                  className="mt-1 sm:mt-0 sm:absolute sm:right-[-16px] sm:top-0 "
                  width={"18px"}
                  src={linkSVG}
                />
              </a>
              <small className="sm:mt-auto text-purple">
                {formatNumber(channelData.subscriberCount)}
                {channelData.subscriberCount > 1000 ||
                channelData.subscriberCount === 0
                  ? " prenumeratorių"
                  : " prenumeratoriai"}
              </small>
            </div>
          </div>
          <div className="text-ellipsis overflow-hidden line-clamp-6 text-xs ">
            {loading ? <Loader /> : <p>{channelData.description}</p>}
          </div>
        </div>
        <div className="absolute top-1 right-2 flex flex-col ml-auto text-right text-xs mr-1">
          <button
            onClick={() => handleUpdateClick(channelData.id)}
            className="flex gap-1"
          >
            <small className=" ">atnaujinta {channelData.lastUpdatedAt}</small>
            <img width={"10px"} src={refreshSVG} alt="" />
          </button>
        </div>
      </div>
      <div className="px-2 py-2 flex ml-1 flex-wrap gap-2">
        {channelData.categories.map((category) => {
          return (
            <div
              key={category.name}
              className={`text-black px-2 rounded-full bg-[#f8efd9]`}
            >
              <Category category={category} />
            </div>
          );
        })}
      </div>
    </div>
  );
}
export default Card;
