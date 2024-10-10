const apiURL = import.meta.env.VITE_APP_API_ADDRESS;

// channel requests

export async function getChannelsCount(queryObject) {
  try {
    const response = await fetch(
      `${apiURL}/api/youtubers/count?ChannelHandle=${
        queryObject.search
      }&SortOrder=${
        queryObject.sortOrder
      }&IncludeCategories=${queryObject.includeCategories.join(
        "&"
      )}&ExcludeCategories=${queryObject.excludeCategories.join("&")}`
    );
    if (response.status >= 200 && response.status < 300) {
      const data = await response.json();
      return data;
    } else {
      return null;
    }
  } catch (e) {
    console.log(e);
  }
}

export async function getChannels(queryObject, pageNumber = 1) {
  try {
    const response = await fetch(
      `${apiURL}/api/youtubers?ChannelHandle=${queryObject.search}&SortOrder=${
        queryObject.sortOrder
      }&PageNumber=${pageNumber}&IncludeCategories=${queryObject.includeCategories.join(
        "&"
      )}&ExcludeCategories=${queryObject.excludeCategories.join("&")}`
    );
    if (response.status >= 200 && response.status < 300) {
      const data = await response.json();
      return data;
    } else {
      return null;
    }
  } catch (e) {
    console.log(e);
  }
}
export async function getChannelById(channelId) {
  try {
    const response = await fetch(`${apiURL}/api/youtubers/${channelId}`);
    if (response.status >= 200 && response.status < 300) {
      const data = await response.json();
      return data;
    } else {
      return null;
    }
  } catch (e) {
    console.log(e);
  }
}

export async function getDailyChannel() {
  try {
    const response = await fetch(`${apiURL}/api/youtubers/daily`);
    if (response.status >= 200 && response.status < 300) {
      const data = await response.json();
      return data;
    } else {
      return null;
    }
  } catch (e) {
    console.log(e);
  }
}
export async function addChannel(channelId, secretPhrase) {
  const token = btoa(secretPhrase);
  try {
    const response = await fetch(`${apiURL}/api/youtubers`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Basic ${token}`,
      },
      body: JSON.stringify(channelId),
    });
    if (response.status >= 200 && response.status < 300) {
      const data = await response.json();
      return data;
    } else {
      return null;
    }
  } catch (e) {
    console.log(e);
  }
}
export async function updateChannel(channelId) {
  try {
    const response = await fetch(`${apiURL}/api/youtubers/${channelId}`, {
      method: "PUT",
    });
    if (response.status >= 200 && response.status < 300) {
      const data = await response.json();
      return data;
    } else {
      return null;
    }
  } catch (e) {
    console.log(e);
  }
}

export async function removeChannel(channelId, secretPhrase) {
  const token = btoa(secretPhrase);
  try {
    const response = await fetch(`${apiURL}/api/youtubers/${channelId}`, {
      method: "DELETE",
      headers: {
        Authorization: `Basic ${token}`,
      },
    });
    if (response.status >= 200 && response.status < 300) {
      return true;
    } else {
      return false;
    }
  } catch (e) {
    console.log(e);
  }
}

export async function removeCategoryFromChannel(
  channelId,
  category,
  secretPhrase
) {
  const token = btoa(secretPhrase);
  try {
    const response = await fetch(`${apiURL}/api/youtubers/${channelId}`, {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Basic ${token}`,
      },
      body: JSON.stringify({
        patchMethod: "Remove",
        category: category,
      }),
    });
    if (response.status >= 200 && response.status < 300) {
      const data = await response.json();
      return data;
    } else {
      return null;
    }
  } catch (e) {
    console.log(e);
  }
}
export async function addCategoryToChannel(channelId, category, secretPhrase) {
  const token = btoa(secretPhrase);
  try {
    const response = await fetch(`${apiURL}/api/youtubers/${channelId}`, {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Basic ${token}`,
      },
      body: JSON.stringify({
        patchMethod: "Add",
        category: category,
      }),
    });
    if (response.status >= 200 && response.status < 300) {
      const data = await response.json();
      return data;
    } else {
      return null;
    }
  } catch (e) {
    console.log(e);
  }
}

// category requests

export async function getAllCategories() {
  try {
    const response = await fetch(`${apiURL}/api/Categories`);
    if (response.status >= 200 && response.status < 300) {
      const data = await response.json();
      return data;
    } else {
      return null;
    }
  } catch (e) {
    console.log(e);
  }
}
export async function addCategory(newCategory, secretPhrase) {
  const token = btoa(secretPhrase);
  try {
    const response = await fetch(`${apiURL}/api/Categories`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Basic ${token}`,
      },
      body: JSON.stringify(newCategory),
    });
    console.log(response);
    if (response.status >= 200 && response.status < 300) {
      const data = await response.json();
      return data;
    } else {
      return null;
    }
  } catch (e) {
    console.log(e);
  }
}
export async function removeCategory(category, secretPhrase) {
  const token = btoa(secretPhrase);
  try {
    const response = await fetch(`${apiURL}/api/Categories/${category}`, {
      method: "DELETE",
      headers: {
        Authorization: `Basic ${token}`,
      },
    });
    if (response.status >= 200 && response.status < 300) {
      return true;
    } else {
      return false;
    }
  } catch (e) {
    console.log(e);
  }
}
export async function getCategory(category) {
  try {
    const response = await fetch(`${apiURL}/api/Categories/${category}`);
    if (response.status >= 200 && response.status < 300) {
      const data = await response.json();
      return data;
    } else {
      return null;
    }
  } catch (e) {
    console.log(e);
  }
}

export async function authorize(secretPhrase) {
  const token = btoa(secretPhrase);
  try {
    const response = await fetch(`${apiURL}/api/Auth/authorize`, {
      method: "POST",
      headers: {
        Authorization: `Basic ${token}`,
      },
    });
    if (response.status >= 200 && response.status < 300) {
      return true;
    } else {
      return false;
    }
  } catch (e) {
    console.log(e);
  }
}
