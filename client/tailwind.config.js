/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      borderRadius: {
        "4xl": "30px",
      },
      colors: {
        "gray-light": "#e8e8e8",
        "gray-dark": "#101010",
        purple: "#9a8be2",
        gold: "#ffab34",
      },
    },
  },
  plugins: [],
};
