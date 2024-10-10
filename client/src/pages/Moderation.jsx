import Search from "../components/Search/Search";
import ModerationCardList from "../components/CardList/ModerationCardList/ModerationCardList";
import AddChannelForm from "../components/Forms/AddChannelForm/AddChannelForm";
import AddCategoryForm from "../components/Forms/AddCategoryForm/AddCategoryForm";
import ModerationCategoryList from "../components/CategoryList/ModerationCategoryList.jsx/ModerationCategoryList";
import { useContext, useEffect } from "react";
import { AppContext } from "../context/AppContext";
import SortSelection from "../components/SortSelection/SortSelection";
import AuthorizationForm from "../components/Forms/AuthorizationForm/AuthorizationForm";
import { ModerationContext } from "../context/ModerationContext";
import Loader from "../components/Loader/Loader";

function Moderation() {
  const {
    channels,
    categories,
    getCategories,
    getChannelsData,
    channelsLoading,
    categoriesLoading,
  } = useContext(AppContext);
  const { authorized } = useContext(ModerationContext);

  return (
    <section className="h-full flex flex-col items-center pt-20">
      {!authorized ? (
        <AuthorizationForm />
      ) : (
        <>
          <article className="2xl:max-w-[30%] max-w-full xl:max-w-[40%] lg:max-w-[60%] md:max-w-[75%] m-2 ">
            <ModerationCategoryList />
            {/* {categories && categories.length ? (
              <ModerationCategoryList />
            ) : (
              !categoriesLoading && (
                <div className="text-white text-3xl text-center">
                  <h1>Kategorijos nerastos. Problema su serveriu</h1>
                </div>
              )
            )}
            {categoriesLoading && <Loader />} */}
            <div className="flex flex-col gap-4 my-2">
              <div className="w-[85%] mx-auto">
                <AddCategoryForm />
              </div>
              <div className="w-[85%] mx-auto">
                <AddChannelForm />
              </div>
            </div>
          </article>
          <div className="2xl:w-[30%] w-full xl:w-[40%] lg:w-[60%] md:w-[75%] m-2">
            <Search />
            <SortSelection />
          </div>
          <article className="w-[70%] m-2">
            <ModerationCardList />
          </article>
        </>
      )}
    </section>
  );
}
export default Moderation;
