import React, { Component } from "react";
import { Modal, Form } from "react-bulma-components";

export class RoomLoginModal extends Component {
  static displayName = RoomLoginModal.name;

  render() {
    return (
      <div hidden={this.props.show}>
        <Modal>
          <Modal.Card>
            <Modal.Card.Header>
              <Modal.Card.Title>{this.props.roomName}</Modal.Card.Title>
            </Modal.Card.Header>
            <Modal.Card.Body>
              <Form.Label>Password for {this.props.roomName}</Form.Label>
              <Form.Input placeholder="Password" type="text" />
            </Modal.Card.Body>
          </Modal.Card>
        </Modal>
      </div>
    );
  }
}
