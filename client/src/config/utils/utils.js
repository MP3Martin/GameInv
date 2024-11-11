import { siteConfig } from '@/config/consts/site';

export const durabilityPercentage = (value) => (value / siteConfig.ushortMax) * 100;
