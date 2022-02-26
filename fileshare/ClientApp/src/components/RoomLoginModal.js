import React, { Component } from "react";
import { Redirect } from 'react-router';
import { Modal, Form, Button } from "react-bulma-components";
import * as roomService from "../Services/roomService";

export class RoomLoginModal extends Component {
  static displayName = RoomLoginModal.name;
  constructor(props) {
    super(props);
    this.state = {
      password: null,
      loggedInRoom: null
    };
  }
  
  renderModal() {
    return (
      <Modal show={this.props.showModal} onClose={() => this.props.onClose()}>
        <Modal.Card>
          <Modal.Card.Header showClose={true}>
            <Modal.Card.Title>{this.props.room.name}</Modal.Card.Title>
          </Modal.Card.Header>
          <Modal.Card.Body>
            <Form.Label>Password for {this.props.room.name}</Form.Label>
            <Form.Input
              placeholder="Password"
              type="text"
              onChange={(p) => { this.state.password = p.target.value}}
            />
          </Modal.Card.Body>
          <Modal.Card.Footer alignContent="center">
            <Button
              color="success"
              onClick={() => 
                this.login(this.props.room.id, this.state.password)
              }
            >
              Submit
            </Button>
          </Modal.Card.Footer>
        </Modal.Card>
      </Modal>
    );
  }

  render() {
    let contents = this.props.room == null ? <div></div> : this.renderModal();
    return (
    <div>
      { contents }
      { this.state.loggedInRoom != null ? <Redirect to={"/room/" + this.state.loggedInRoom}/> : <div/>}
    </div>);
  }

  async login(roomId, password) {
    await roomService.loginToRoom(roomId, password);
    this.props.onClose();
    this.setState({loggedInRoom: roomId});
  }
}
