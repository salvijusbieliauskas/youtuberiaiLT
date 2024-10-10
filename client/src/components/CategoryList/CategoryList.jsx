import Category from "../Category/Category";
import { useContext, useEffect } from "react";
import { AppContext } from "../../context/AppContext";
import Loader from "../Loader/Loader";
function CategoryList() {
  const {
    handleCategory,
    searchQuery,
    categories,
    getCategories,
    categoriesLoading,
  } = useContext(AppContext);
  useEffect(() => {
    getCategories();
  }, []);

  return (
    <>
      {categories && categories.length ? (
        <div className=" px-2 py-2 flex flex-wrap gap-2">
          {categories.map((category) => {
            return (
              <button
                onClick={() => handleCategory(category.name)}
                className={`${
                  searchQuery.includeCategories.includes(category.name)
                    ? "bg-purple border border-[#7f74c7] text-white"
                    : searchQuery.excludeCategories.includes(category.name)
                    ? "bg-gold border border-[#e6931d] text-white"
                    : "bg-gray-light border border-white"
                } px-2 rounded-full `}
                key={category.name}
              >
                <Category category={category} />
              </button>
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
export default CategoryList;
