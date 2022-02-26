import React, { Component } from 'react';
import { Heading, Level, Table, Button } from 'react-bulma-components';
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
      <Table hoverable="true" striped="true" bordered="true">
        <tbody>
          {rooms.map(room =>
            <tr key={room.id}>
              <td>{room.name}</td>
            </tr>
          )}
        </tbody>
      </Table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : RoomList.renderRoomsTable(this.state.rooms);

    return (
      <div>
        <Level>
          <Level.Side align="left">
            <Level.Item>
              <Heading size="3">Rooms</Heading>
            </Level.Item>
          </Level.Side>
          <Level.Side align="right">
            <Level.Item>
              <Button color="primary">+ Add Room</Button>
            </Level.Item>
          </Level.Side>
        </Level>
        {contents}
      </div>
    );
  }

  async loadRooms() {
    let data = await roomService.getRooms();
    this.setState({rooms: data, loading: false});
  }
}
