import { useContext, useState } from "react";
import { AppContext } from "../../../context/AppContext";
import { addCategory } from "../../../api";
import { ModerationContext } from "../../../context/ModerationContext";

function AddCategoryForm() {
  const { categories, setCategories } = useContext(AppContext);
  const { secretPhrase } = useContext(ModerationContext);
  const [newCategory, setNewCategory] = useState("");
  const [showMessage, setShowMessage] = useState(false);
  const [error, setError] = useState({
    success: false,
    message: "",
  });
  async function handleAddCategory(e) {
    e.preventDefault();
    const responseCategory = await addCategory(newCategory, secretPhrase);
    if (responseCategory !== null) {
      setError({ success: true, message: "Sėkmingai pridėta." });
      setShowMessage(true);
      setCategories([...categories, responseCategory]);
    } else {
      setShowMessage(true);
      setError({ success: false, message: "Kategorijos pridėti nepavyko." });
    }
  }
  function handleCategoryInputChange(e) {
    setNewCategory(e.target.value);
  }
  return (
    <>
      {showMessage && (
        <div className="text-center my-2">
          <h3
            className={`${
              error.success ? "text-green-600" : "text-red-600"
            } text-2xl`}
          >
            {error.message}
          </h3>
        </div>
      )}
      <form
        onSubmit={handleAddCategory}
        className="rounded-full bg-slate-300 text-xl flex"
      >
        <input
          onChange={handleCategoryInputChange}
          className="rounded-l-full py-1 bg-slate-100 px-3 w-full"
          type="text"
          placeholder="Kategorija..."
        />
        <button
          type="submit"
          className=" px-2 border-l border-black rounded-r-full  hover:bg-emerald-200 active:bg-emerald-400 "
        >
          Pridėti
        </button>
      </form>
    </>
  );
}
export default AddCategoryForm;
