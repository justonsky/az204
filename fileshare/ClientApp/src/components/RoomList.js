import React, { Component } from "react";
import { Heading, Level, Table, Button } from "react-bulma-components";
import * as roomService from "../Services/roomService";
import { RoomLoginModal } from "./RoomLoginModal";

export class RoomList extends Component {
  static displayName = RoomList.name;

  constructor(props) {
    super(props);
    this.state = {
      rooms: [],
      loading: true,
      selectedRoom: null
    };
  }

  componentDidMount() {
    this.loadRooms();
  }

  renderRoomsTable(rooms) {
    return (
      <div>
        <Table hoverable="true" striped="true" bordered="true">
          <tbody>
            {rooms.map((room) => (
              <tr key={room.id}>
                <td onClick={() => this.surfaceLoginModal(room)}>{room.name}</td>
              </tr>
            ))}
          </tbody>
        </Table>
        <RoomLoginModal 
          showModal={this.state.selectedRoom != null} 
          room={this.state.selectedRoom}
          onClose={() => this.setState({selectedRoom: null})} />
      </div>
    );
  }

  render() {
    let contents = this.state.loading ? (
      <p>
        <em>Loading...</em>
      </p>
    ) : (
      this.renderRoomsTable(this.state.rooms)
    );

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
              <Button
                color="primary"
                disabled={this.state.loading}
              >
                + Add Room
              </Button>
            </Level.Item>
          </Level.Side>
        </Level>
        {contents}
      </div>
    );
  }

  async loadRooms() {
    let data = await roomService.getRooms();
    this.setState({ rooms: data, loading: false });
  }

  surfaceLoginModal(room) {
    this.setState({selectedRoom: room});
  }
}
