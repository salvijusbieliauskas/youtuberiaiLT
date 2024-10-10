export const formatNumber = (number) => {
  let formattedNum = number;
  if (number < 1000) {
    return formattedNum;
  } else if (number >= 1000 && number < 1_000_000) {
    return removeDecimalZeroes((number / 1000).toFixed(2)) + "K";
  } else if (number >= 1_000_000 && number < 1_000_000_000) {
    return removeDecimalZeroes((number / 1_000_000).toFixed(2)) + "M";
  } else if (number >= 1_000_000_000 && number < 1_000_000_000_000) {
    return removeDecimalZeroes((number / 1_000_000_000).toFixed(2)) + "B";
  } else if (number >= 1_000_000_000_000 && number < 1_000_000_000_000_000) {
    return removeDecimalZeroes((number / 1_000_000_000_000).toFixed(2)) + "T";
  }
};

export const formatRatio = (ratio) => {
  return (Math.round(ratio * 100) / 100).toFixed(2);
};

function removeDecimalZeroes(num) {
  let tempNum = num.toString();
  let lastIndex = tempNum.length - 1;
  let secondLastIndex = tempNum.length - 2;
  if (tempNum[lastIndex] == 0) {
    tempNum = tempNum.substr(0, lastIndex);
  }
  if (tempNum[secondLastIndex] == 0) {
    tempNum = tempNum.substr(0, secondLastIndex);
  }
  return Number(tempNum);
}
