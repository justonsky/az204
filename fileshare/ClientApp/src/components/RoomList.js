import React, { Component } from "react";
import { Heading, Level, Table, Button } from "react-bulma-components";
import * as roomService from "../Services/roomService";
import { RoomLoginModal as RoomLoginModal } from "./RoomLoginModal";

export class RoomList extends Component {
  static displayName = RoomList.name;

  constructor(props) {
    super(props);
    this.state = {
      rooms: [],
      loading: true,
      showLoginModal: false,
      loggingIntoRoomName: null
    };
  }

  componentDidMount() {
    this.loadRooms();
  }

  renderRoomsTable(rooms) {
    return (
      <Table hoverable="true" striped="true" bordered="true">
        <tbody>
          {rooms.map((room) => (
            <tr key={room.id}>
              <td onClick={() => this.surfaceLoginModal(room.name)}>{room.name}</td>
            </tr>
          ))}
        </tbody>
      </Table>
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
                onClick={() => this.surfaceLoginModal("oo")}
                disabled={this.state.loading}
              >
                + Add Room
              </Button>
            </Level.Item>
          </Level.Side>
        </Level>
        {contents}
        <RoomLoginModal show={this.state.showLoginModal} roomName={this.state.loggingIntoRoomName} />
      </div>
    );
  }

  async loadRooms() {
    let data = await roomService.getRooms();
    this.setState({ rooms: data, loading: false });
  }

  surfaceLoginModal(roomName) {
    this.setState({showLoginModal: true, loggingIntoRoomName: roomName})
  }
}
