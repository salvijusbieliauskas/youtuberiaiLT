import { createContext, useEffect, useState } from "react";
import { getAllCategories, getChannels } from "../api";

export const AppContext = createContext(null);
export function AppProvider({ children }) {
  const [searchQuery, setSearchQuery] = useState({
    search: "",
    includeCategories: [],
    excludeCategories: [],
    sortOrder: "",
  });
  const [channels, setChannels] = useState([]);
  const [categories, setCategories] = useState([]);
  const [channelsLoading, setChannelsLoading] = useState(false);
  const [categoriesLoading, setCategoriesLoading] = useState(false);
  function handleCategory(categoryName) {
    const prevIncludeCategories = searchQuery.includeCategories;
    const prevExcludeCategories = searchQuery.includeCategories;
    if (
      !searchQuery.includeCategories.includes(categoryName) &&
      !searchQuery.excludeCategories.includes(categoryName)
    ) {
      setSearchQuery((prevQuery) => ({
        ...prevQuery,
        includeCategories: [...prevIncludeCategories, categoryName],
      }));
      return;
    }
    if (searchQuery.includeCategories.includes(categoryName)) {
      const tempIncludeCategories = searchQuery.includeCategories.filter(
        (catName) => catName !== categoryName
      );

      setSearchQuery((prevQuery) => ({
        ...prevQuery,
        includeCategories: tempIncludeCategories,
        excludeCategories: [...prevExcludeCategories, categoryName],
      }));
      return;
    }
    if (searchQuery.excludeCategories.includes(categoryName)) {
      const tempExcludeCategories = searchQuery.excludeCategories.filter(
        (catName) => catName !== categoryName
      );
      setSearchQuery((prevQuery) => ({
        ...prevQuery,
        excludeCategories: tempExcludeCategories,
      }));
      return;
    }
  }
  function handleSearchInputChange(e) {
    setSearchQuery((prevQuery) => ({
      ...prevQuery,
      search: e.target.value,
    }));
  }

  async function handleSearchTermChange(searchTerm) {
    setSearchQuery((prevQuery) => ({
      ...prevQuery,
      search: searchTerm,
    }));
  }

  async function handleSortingChange(sortBy) {
    setSearchQuery((prevQuery) => ({
      ...prevQuery,
      sortOrder: sortBy,
    }));
  }

  function removeCategoryFromSearch(category) {
    const tempIncludeCategories = searchQuery.includeCategories.filter(
      (catName) => catName !== category
    );
    const tempExcludeCategories = searchQuery.excludeCategories.filter(
      (catName) => catName !== category
    );
    setSearchQuery((prevQuery) => ({
      ...prevQuery,
      includeCategories: tempIncludeCategories,
      excludeCategories: tempExcludeCategories,
    }));
  }

  function getChannelsData() {
    setChannelsLoading(true);
    getChannels(searchQuery).then((data) => {
      if (data) {
        setChannels(data);
      } else {
        setChannels(channels);
      }
      setChannelsLoading(false);
    });
  }
  function getMoreChannels(pageNumber = 1) {
    getChannels(searchQuery, pageNumber).then((data) => {
      if (data) {
        setChannels((prevChannels) => [...prevChannels, ...data]);
      } else {
        setChannels([...channels]);
      }
    });
  }
  function getCategories() {
    setCategoriesLoading(true);
    getAllCategories().then((data) => {
      setCategories(data);
      setCategoriesLoading(false);
    });
  }

  return (
    <AppContext.Provider
      value={{
        searchQuery,
        channels,
        categories,
        channelsLoading,
        categoriesLoading,
        setChannelsLoading,
        setCategories,
        setChannels,
        handleCategory,
        handleSearchInputChange,
        handleSearchTermChange,
        removeCategoryFromSearch,
        getChannelsData,
        getCategories,
        handleSortingChange,
        getMoreChannels,
      }}
    >
      {children}
    </AppContext.Provider>
  );
}
