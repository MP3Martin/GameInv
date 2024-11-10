/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  output: 'export',
  images: {
    unoptimized: true
  },
  sassOptions: {
    silenceDeprecations: ['legacy-js-api']
  }
};

module.exports = nextConfig;
