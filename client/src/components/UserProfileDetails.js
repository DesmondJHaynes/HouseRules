import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getUserProfile } from "../managers/userProfileManger.js";
import { Table } from "reactstrap";

export const UserProfileDetails = () => {
  const { id } = useParams();
  const [profile, setProfile] = useState();

  useEffect(() => {
    getUserProfile(id).then(setProfile);
  }, []);

  if (!profile) {
    return null;
  }

  return (
    <>
      <h2>User: {profile.firstName}</h2>
      <Table>
        <thead>
          <tr>
            <th>Last Name</th>
            <th>First Name</th>
            <th>Address Name</th>
          </tr>
        </thead>
        <tbody>
          {
            <tr>
              <td>{profile.lastName}</td>
              <td>{profile.firstName}</td>
              <td>{profile.address}</td>
              <td></td>
            </tr>
          }
        </tbody>
      </Table>
      <h3>Chores</h3>
      <div className="sidebyside">
        <Table>
          <thead>
            <tr>
              <th>Assigned</th>
            </tr>
          </thead>
          <tbody>
            {profile.choreAssignments.map((ca) => (
              <tr key={`ca--${ca.id}`}>
                <td>{ca.chore.name}</td>
              </tr>
            ))}
          </tbody>
        </Table>
        <Table>
          <thead>
            <tr>
              <th>Completed</th>
              <th>Completion Date</th>
            </tr>
          </thead>
          <tbody>
            {profile.choreCompletions.map((cp) => (
              <tr key={`cp--${cp.id}`}>
                <td>{cp.chore.name}</td>
                <td>{cp.completedOn}</td>
              </tr>
            ))}
          </tbody>
        </Table>
      </div>
    </>
  );
};
