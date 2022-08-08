import { ClientConfig } from "./models/configs/client-config";
export { };

declare global {
  interface Window {
    clientConfig: ClientConfig;
    translations: any;
    lang: string;
  }
}
