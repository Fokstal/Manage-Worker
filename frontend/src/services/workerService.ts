import worker from "../types/worker";

class WorkerService {
  private url = 'http://localhost:5177/worker';

  public getWorkers = async () : Promise<Array<worker>> => {
    const res = await fetch(this.url, {
      method : 'GET',
      headers : {
        'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('jwt-token')!)['access_token']
      },
    });

    if (!res.ok) throw new Error(res.statusText);

    return await res.json();
  }

  public createWorker = async (worker : any) : Promise<any> => {
    const res = await fetch(this.url, {
      method : 'POST',
      headers : {
        // 'Content-Type' : 'multipart/form-data',
        'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('jwt-token')!)['access_token']
      },
      body : worker
    });

    if (!res.ok) throw new Error(res.statusText);

    return await res.json();
  }

  public getWorker = async (id : number) : Promise<worker> => {
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

  public editWorker = async (id : number, worker : FormData) : Promise<void> => {
    const res = await fetch(`${this.url}/${id}`, {
      method : 'PUT',
      headers : {
        'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('jwt-token')!)['access_token']
      },
      body : worker
    });

    if (!res.ok) throw new Error(res.statusText);
  }

  public deleteWorker = async (id : number) : Promise<void> => {
    const res = await fetch(`${this.url}/${id}`, {
      method : 'DELETE',
      headers : {
        'Authorization' : 'Bearer ' + JSON.parse(localStorage.getItem('jwt-token')!)['access_token']
      }
    });

    if (!res.ok) throw new Error(res.statusText);
  }
}

export default WorkerService;