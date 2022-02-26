import React, { Component } from 'react';
import * as roomService from '../Services/roomService';

export class RoomList extends Component {
  static displayName = RoomList.name;

  constructor(props) {
    super(props);
    this.state = { rooms: [], loading: true };
  }

  componentDidMount() {
    this.loadRooms();
  }

  static renderRoomsTable(rooms) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
          </tr>
        </thead>
        <tbody>
          {rooms.map(room =>
            <tr key={room.id}>
              <td>{room.id}</td>
              <td>{room.name}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : RoomList.renderRoomsTable(this.state.rooms);

    return (
      <div>
        <h1 id="tabelLabel" >Rooms</h1>
        {contents}
      </div>
    );
  }

  async loadRooms() {
    let data = await roomService.getRooms();
    this.setState({rooms: data, loading: false});
  }
}
