class StuffService {
  private readonly url = 'http://localhost:5177/api/StuffAPI';

  public getStuffs = async () : Promise<Array<any>> => {
    const res = await fetch(this.url, {
      method : 'GET',
      headers : {
        'Content-Type' : 'application/json' 
      },
    });

    if (!res.ok) throw new Error(await res.json());

    return await res.json();
  }
}

export default StuffService;