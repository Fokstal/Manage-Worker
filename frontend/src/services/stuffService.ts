import stuff from "../types/stuff";

class StuffService {
  private readonly url = 'http://localhost:5177/stuff';

  public getStuffs = async () : Promise<Array<stuff>> => {
    const res = await fetch(this.url, {
      method : 'GET',
      headers : {
        'Content-Type' : 'application/json',
        'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('jwt-token')!)['access_token']
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
        'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('jwt-token')!)['access_token']
      }
    });

    if (!res.ok) throw new Error(await res.json());

    return await res.json();
  }

  public createStuff = async (stuff : stuff) : Promise<stuff> => {
    const res = await fetch(this.url, {
      method : 'POST',
      headers : {
        'Content-Type' : 'application/json',
        'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('jwt-token')!)['access_token']
      },
      body : JSON.stringify({
        "name": stuff.name,
      })
    });
    if (!res.ok) throw new Error(await res.json());

    return await res.json();
  }

  public deleteStuff = async (id : number) : Promise<void> => {
    const res = await fetch(`${this.url}/${id}`, {
      method : 'DELETE',
      headers : {
        'Content-Type' : 'application/json',
        'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('jwt-token')!)['access_token']
      }
    });

    if (!res.ok) throw new Error(res.statusText);
  }

  public ChangeStuff = async (id : number, stuff : stuff) : Promise<void> => {
    const res = await fetch(`${this.url}/${id}`, {
      method : 'PUT',
      headers : {
        'Content-Type' : 'application/json',
        'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('jwt-token')!)['access_token']
      },
      body : JSON.stringify({
        "id" : id,
        "name": stuff.name,
      })
    });

    if (!res.ok) throw new Error(res.statusText);
  }
}

export default StuffService;