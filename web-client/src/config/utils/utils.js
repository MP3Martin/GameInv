import Gradient from 'javascript-color-gradient';

import { siteConfig } from '@/config/consts/site';

export const durabilityPercentage = (value) => (value / siteConfig.ushortMax) * 100;

/**
 * Converts 0-100 to red -> yellow -> green
 */
export const percentageToDamageColor = (percentage) => {
  // const green = (percentage * 255) / 100;
  // const red = 255 - ((percentage * 255) / 100);
  //
  // return `#${rgbHex(red, green, 0)}`;

  return new Gradient()
    .setColorGradient('#cc2300', '#cc6800', '#cca800', '#b8cc00', '#75cc00', '#2fcc00', '#02cc00')
    .setMidpoint(100 + 1).getColor(percentage);
};
