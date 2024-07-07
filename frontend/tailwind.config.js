/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts}"],
  theme: {
    extend: {
      colors: {
        tw: {
          primary: "#1f1f23",
          secondary: "#18181b",
          tertiary: "#35353b",
          accent: "#f5f5f5",
          dark: "#0e0e10",
          light: "#ffffff",
          danger: "#ff0000",
          success: "#00ff00",
          warning: "#ffff00",
          info: "#0000ff",
          gray: {
            50: "#f9fafb",
            100: "#f3f4f6",
            200: "#e5e7eb",
            300: "#d1d5db",
            400: "#9ca3af",
            500: "#6b7280",
            600: "#4b5563",
            700: "#374151",
            800: "#1f2937",
            900: "#111827",
          },
        },
      },
    },
  },
  plugins: [],
};
