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
}

export default WorkerService;