import stuff from "../types/stuff";

class StuffService {
  private readonly url = 'http://localhost:5177/api/StuffAPI';

  public getStuffs = async () : Promise<Array<stuff>> => {
    const res = await fetch(this.url, {
      method : 'GET',
      headers : {
        'Content-Type' : 'application/json',
        'Authorization' : JSON.parse(localStorage.getItem('jwt-token')!)['access_token']
      },
    });

    if (!res.ok) throw new Error(await res.json());

    return await res.json();
  }

  public getStuff = async (id : number) : Promise<stuff> => {
    const res = await fetch(`${this.url}/${id}`, {
      method : 'GET',
      headers : {
        'Content-Type' : 'application/json',
        'Authorization' : JSON.parse(localStorage.getItem('jwt-token')!)['access_token']
      }
    });

    if (!res.ok) throw new Error(await res.json());

    return await res.json();
  }
}

export default StuffService;