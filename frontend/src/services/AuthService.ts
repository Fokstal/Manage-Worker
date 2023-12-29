class AuthService {
  private url = 'http://localhost:5177/AccountAPI';
  private key = 'KeyToAdd99Key';

  public register = async () : Promise<string> => {
    const res = await fetch(`${this.url}/${this.key}`, {
      method: 'POST',
    });

    if (!res.ok) throw new Error(`Error while register ${res.statusText}`);

    return await res.json();
  }

  public login = async () : Promise<any> => {

  }

  public logout = async () : Promise<any> => {

  }

}

export default AuthService;