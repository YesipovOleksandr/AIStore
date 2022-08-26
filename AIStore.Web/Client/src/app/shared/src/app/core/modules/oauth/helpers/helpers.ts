export default class Helpers {
  public static MapParams(params: object, asJson = false): any {
    let mappedParams: any;
    if (asJson) {
      mappedParams = JSON.stringify(params);
    } else {
      mappedParams = Object.keys(params).reduce(
        (array, key) => {
          array.push(`${key}=${params[key]}`);
          return array;
        },
        []).join('&');
    }
    return mappedParams;
  }
  public static Contains(input: string, search: string): boolean {
    return input.toLowerCase().indexOf(search) >= 0;
  }
}
