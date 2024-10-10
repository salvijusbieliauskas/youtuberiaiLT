import { useContext, useRef } from "react";
import searchSVG from "../../assets/icons/search.svg";
import { AppContext } from "../../context/AppContext";
function Search() {
  const inputRef = useRef(null);
  const { handleSearchTermChange } = useContext(AppContext);

  function handleSearchSubmit(e) {
    e.preventDefault();
    const searchTerm = inputRef.current.value;
    handleSearchTermChange(searchTerm);
  }

  return (
    <form onSubmit={handleSearchSubmit} className="w-full">
      <div className="w-full flex bg-[#e8e8e8] rounded-full ">
        <input
          ref={inputRef}
          className="py-1 focus:outline-none border border-r-0 focus:border focus:border-r-0 focus:border-[#e1a44f] w-full rounded-l-full  bg-inherit pl-3 text-xl font-normal"
          type="text"
          placeholder="Paieška..."
        />
        <button
          type="submit"
          className="border border-r-0 border-y-0 px-5 rounded-r-full border-black hover:bg-[#998be24d] hover:border-[#e1a44f] active:bg-[#8573dea6]  opacity-60 hover:opacity-100"
        >
          <img className="w-6" src={searchSVG} alt="" />
        </button>
      </div>
      <div className="h-[2px] bg-[#e8e8e8] w-[85%] rounded-full mt-2 mx-auto"></div>
    </form>
  );
}
export default Search;
