import React, { Component } from 'react';

export class Room extends Component {
  static displayName = Room.name;

  constructor(props) {
    super(props);
    this.state = { files: [], loading: true };
  }

  componentDidMount() {
    this.getFilesForRoom();
  }

  static renderFilesTable(files) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
          </tr>
        </thead>
        <tbody>
          {files.map(file =>
            <tr key={file.id}>
              <td>{file.id}</td>
              <td>{file.name}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderFilesTable(this.state.files);

    return (
      <div>
        <h1 id="tabelLabel" >Room {this.props.roomName}</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async getFilesForRoom() {
    const response = await fetch(`${this.props.roomId}/files`);
    const data = await response.json();
    this.setState({ files: data, loading: false });
  }
}
