export const safeJsonParse = <T> (str: string) => {
  try {
    const jsonValue: T = JSON.parse(str);

    return jsonValue;
  } catch {
    return undefined;
  }
};

export const isValidWebSocketURI = (uri: string): boolean => {
  try {
    const url = new URL(uri);

    return url.protocol === 'ws:' || url.protocol === 'wss:';
  } catch {
    return false;
  }
};
