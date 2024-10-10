import Category from "../../Category/Category";
import { useContext, useEffect } from "react";
import { AppContext } from "../../../context/AppContext";
import { getAllCategories, removeCategory } from "../../../api";
import { ModerationContext } from "../../../context/ModerationContext";
import Loader from "../../Loader/Loader";
function ModerationCategoryList() {
  const {
    handleCategory,
    searchQuery,
    setCategories,
    removeCategoryFromSearch,
    categories,
    categoriesLoading,
    getCategories,
  } = useContext(AppContext);
  const { secretPhrase } = useContext(ModerationContext);

  async function handleRemoveCategory(category) {
    const confirm = window.confirm(`Ištrinti ${category} kategoriją?`);
    if (confirm) {
      const removed = await removeCategory(category, secretPhrase);
      if (removed) {
        removeCategoryFromSearch(category);
        const data = await getAllCategories();
        setCategories(data);
      }
    }
  }
  useEffect(() => {
    getCategories();
  }, []);

  return (
    <>
      {categories && categories.length ? (
        <div className=" px-2 py-2 flex flex-wrap gap-2">
          {categories.map((category) => {
            return (
              <div
                key={category.name}
                className={`${
                  searchQuery.includeCategories.includes(category.name)
                    ? "bg-purple border border-[#7f74c7] text-white"
                    : searchQuery.excludeCategories.includes(category.name)
                    ? "bg-gold border border-[#e6931d] text-white"
                    : "bg-gray-light border border-white"
                } flex rounded-full`}
              >
                <button
                  onClick={() => handleCategory(category.name)}
                  className={` px-2 rounded-l-full `}
                >
                  <Category category={category} />
                </button>
                <button
                  onClick={() => handleRemoveCategory(category.name)}
                  className="bg-rose-600 hover:bg-red-500 active:bg-red-400 ml-1 px-2 rounded-r-full"
                >
                  X
                </button>
              </div>
            );
          })}
        </div>
      ) : (
        !categoriesLoading && (
          <div className="text-white text-3xl text-center">
            <h1>Kategorijos nerastos. Problema su serveriu</h1>
          </div>
        )
      )}
      {categoriesLoading && <Loader />}
    </>
  );
}
export default ModerationCategoryList;
