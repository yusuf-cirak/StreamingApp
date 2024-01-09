/** @type {import('tailwindcss').Config} */
module.exports = {
  presets: [require("@spartan-ng/ui-core/hlm-tailwind-preset")],
  content: ["./src/**/*.{html,ts}", "./libs/spartan/**/*.{html,ts}"],
  theme: {
    extend: {},
  },
  plugins: [],
};
