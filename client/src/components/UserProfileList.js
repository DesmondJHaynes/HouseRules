import { useEffect, useState } from "react";
import { Table } from "reactstrap";
import { getUserProfileList } from "../managers/userProfileManger.js";
import { Link } from "react-router-dom";

export const UserProfileList = () => {
  const [profilelist, setProfileList] = useState();

  useEffect(() => {
    getUserProfileList().then(setProfileList);
  }, []);

  if (!profilelist) {
    return null;
  }

  return (
    <>
      <h1>User Profile List!</h1>
      <Table>
        <thead>
          <tr>
            <th>Last Name</th>
            <th>First Name</th>
          </tr>
        </thead>
        <tbody>
          {profilelist.map((p) => (
            <tr key={`profile--${p.id}`}>
              <td>{p.lastName}</td>
              <td>{p.firstName}</td>
              <td>
                <Link to={`/userprofiles/${p.id}`}>
                  <button>Details</button>
                </Link>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </>
  );
};
