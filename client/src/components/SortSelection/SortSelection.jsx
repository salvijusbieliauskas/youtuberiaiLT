import { useContext } from "react";
import { AppContext } from "../../context/AppContext";

function SortSelection() {
  const { handleSortingChange, searchQuery } = useContext(AppContext);

  return (
    <form>
      <select
        onChange={(e) => handleSortingChange(e.target.value)}
        value={searchQuery.sortOrder}
        id="sort-select"
      >
        <option value="Default">Rūšiuoti</option>
        <option value="ByHandleAscending">Pavadinimas A-Z</option>
        <option value="ByHandleDescending">Pavadinimas Z-A</option>
        <option value="BySubCountAscending">Prenumeratų sk. Min-Max</option>
        <option value="BySubCountDescending">Prenumeratų sk. Max-Min</option>
      </select>
    </form>
  );
}
export default SortSelection;
