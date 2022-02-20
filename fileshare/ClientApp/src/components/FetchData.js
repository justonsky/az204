import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { rooms: [], loading: true };
  }

  componentDidMount() {
    this.populateWeatherData();
  }

  static renderForecastsTable(rooms) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Room</th>
            <th>ID. (C)</th>
          </tr>
        </thead>
        <tbody>
          {rooms.map(room =>
            <tr key={room.id}>
              <td>{room.name}</td>
              <td>{room.id}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderForecastsTable(this.state.rooms);

    return (
      <div>
        <h1 id="tabelLabel" >Room list</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch('room');
    const data = await response.json();
    this.setState({ rooms: data, loading: false });
  }
}
